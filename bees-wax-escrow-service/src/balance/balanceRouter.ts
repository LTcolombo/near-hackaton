import express, { Request, Response } from "express";
import { connect } from "near-api-js";
import { rpcConfig } from "../common/near.config";
import { getCollection } from "../db/deposits";

export const balanceRouter = express.Router();

let account;
var initAccount = async () => {
    if (account)
        return true;

    const near = await connect(rpcConfig);
    const ACCOUNT_ID = 'nickbalyanitsa.testnet';
    account = await near.account(ACCOUNT_ID);

    return true;
}

balanceRouter.get("/", async (req: Request, res: Response) => {
    const deposit = (await (await getCollection()).findOne({ wallet: req.query["id"] }));
    if (deposit)
        res.status(200).send(deposit.value.toString());
    else
        res.status(404).send('not found');
});

balanceRouter.post("/", async (req: Request, res: Response) => {
    try {
        if (!req.body.id || !req.body.value) {
            res.status(400).send('bad request');
            return;
        }

        const deposit = (await (await getCollection()).findOne({ wallet: req.body.id }));

        if (deposit && deposit.value >= req.body.value) {
            res.status(200).send(
                await (await getCollection()).updateOne({ wallet: req.body.id }, { $inc: { value: -req.body.value } }) &&
                await initAccount() &&
                await account.sendMoney(req.body.id, req.body.value));
            return;
        }
        else
            res.status(404).send('wallet not found');

    } catch (e: any) {
        res.status(500).send(e.message);
    }
});
