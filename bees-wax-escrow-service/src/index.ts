import express from "express";
import { connectToDatabase } from "./db/deposits";
import { DepositListener } from "./balance/depositListener";
import { nftRouter } from "./nft/NFTRouter";
import { balanceRouter } from "./balance/balanceRouter";
import cors from "cors";

import Bugsnag from 'bugsnag'
import { Escrow } from "./escrow/escrow.service";
Bugsnag.register('9e4fbc24e93f3b61af7ba23bd26be0f0');

const app = express();

app.use(
    cors({
        origin: (origin, callback) => callback(null, true),
        credentials: true,
    })
);

app.use(express.json());

app.use("/api/nft", nftRouter);
app.use("/api/balance", balanceRouter);

app.listen(8084, async () => {
    console.log(`Listening on port ${8084}`);
});



DepositListener.getTransactions(null, "nickbalyanitsa.testnet");

connectToDatabase();

new Escrow();
