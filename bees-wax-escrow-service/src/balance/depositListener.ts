import { archConfig } from "../common/near.config";
import { connect } from "near-api-js";
import { text } from "stream/consumers";
import { getCollection } from "../db/deposits";
import { ObjectId } from "mongodb";


export class DepositListener {

    static delay = (ms) => new Promise(res => setTimeout(res, ms));

    static async getTransactions(startBlock, accountId) {

        let deposits = {};

        const near = await connect(archConfig);

        var latestBlock = (await near.connection.provider.status()).sync_info.latest_block_hash;

        // creates an array of block hashes for given range
        const blockArr = [];
        let blockHash = latestBlock;
        do {
            if (latestBlock == startBlock)
                break;//all processed, nothign to do here (no new blocks addded since last pass)
            const currentBlock = await await near.connection.provider.block({ blockId: blockHash });
            blockArr.push(currentBlock.header.hash);
            blockHash = currentBlock.header.prev_hash;
        } while (blockHash !== startBlock && startBlock != null);

        // returns block details based on hashes in array
        const blockDetails = await Promise.all(
            blockArr.map((blockId) =>
                near.connection.provider.block({
                    blockId,
                })
            )
        );

        // returns an array of chunk hashes from block details
        const chunkHashArr = blockDetails.flatMap((block) =>
            block.chunks.map(({ chunk_hash }) => chunk_hash)
        );

        //returns chunk details based from the array of hashes
        const chunkDetails = await Promise.all(
            chunkHashArr.map(chunk => near.connection.provider.chunk(chunk))
        );

        // checks chunk details for transactions
        // if there are transactions in the chunk we
        // find ones associated with passed accountId


        const transactions: any = chunkDetails.flatMap((chunk) =>
            (chunk.transactions || []).filter((tx: any) => tx.receiver_id === accountId)
        );

        //[{ "Transfer": { "deposit": "1" } }]
        for (let tx of transactions)
            for (let action of (<any>tx).actions) {
                if (action.Transfer)
                    if (action.Transfer.deposit) {
                        if (!deposits[tx.signer_id])
                            deposits[tx.signer_id] = parseInt(action.Transfer.deposit);
                        else
                            deposits[tx.signer_id] += parseInt(action.Transfer.deposit);
                    }
            }

        console.log(deposits);

        for (var id of Object.keys(deposits)) {

            const deposit = (await (await getCollection()).findOne({ wallet: id }));
            if (deposit)
                console.log(await (await getCollection()).updateOne({ wallet: id }, { $inc: { value: deposits[id] } }, true));
            else
                console.log(await (await getCollection()).insertOne({ wallet: id, value: deposits[id] }));
        }

        await DepositListener.delay(1000);
        this.getTransactions(latestBlock, accountId);
    }
}
