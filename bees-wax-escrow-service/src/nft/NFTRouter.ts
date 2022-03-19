import express, { Request, Response } from "express";
import { connect, Contract } from "near-api-js";
import crypto from "crypto";

import { rpcConfig } from "../common/near.config"

export const nftRouter = express.Router();

const ACCOUNT_ID = 'nickbalyanitsa.testnet';

var contract: any;

var initContract = async () => {
    if (contract)
        return true;

    const near = await connect(rpcConfig);
    const account = await near.account(ACCOUNT_ID);

    contract = new Contract(
        account, // the account object that is connecting
        ACCOUNT_ID,
        {
            viewMethods: ["nft_tokens_for_owner"], // view methods do not change state but usually return a value
            changeMethods: ["new_default_meta", "nft_mint"]//, // change methods modify state
        }
    );
    return true;
}

nftRouter.get("/", async (req: Request, res: Response) => {
    res.status(200).send(await initContract() && await contract.nft_tokens_for_owner({ account_id: req.query["id"] }));
});

nftRouter.post("/", async (req: Request, res: Response) => {
    try {

        if (!req.body.id) {
            res.status(400).send('bad request');
            return;
        }

        res.status(200).send(await initContract() && await contract.nft_mint({
            args: {
                "token_id": crypto.randomBytes(12).toString('hex'),
                "receiver_id": req.body.id,
                "token_metadata": getRandomMetadata()
            },

            gas: "300000000000000", // attached GAS (optional)
            amount: "10000000000000000000000" // attached deposit in yoctoNEAR (optional)
        }));
    } catch (e: any) {
        res.status(500).send(e.message);
    }
});

//serve it from external config
var templates = [
    {
        title: "Hatch Boost",
        description: "Reduces Hatch Time",
        media: "https://bafkreidps3om43em6ayzcvn5p6fdaornmdkrow2ab6kamfcbkcgoqbr7g4.ipfs.nftstorage.link",
        extra: "hatch"
    }, {
        title: "Damage Boost",
        description: "Increases Damage",
        media: "https://bafkreihwomwmvstvkrpazgrqa4l6dkam6nywvafxmlptbwcd2y4orrwl5y.ipfs.nftstorage.link/",
        extra: "damage"
    }, {
        title: "HP Boost",
        description: "Increases Health Points",
        media: "https://bafkreidps3om43em6ayzcvn5p6fdaornmdkrow2ab6kamfcbkcgoqbr7g4.ipfs.nftstorage.link/",
        extra: "hp"
    }, {
        title: "Harvest Boost",
        description: "Increases Harvest Speed",
        media: "https://bafkreigwemt362pcybuzspafz3mwgdasrnat3dnbo225ehngt6ngvknq7m.ipfs.nftstorage.link/",
        extra: "harvest"
    }, {
        title: "Armour Boost",
        description: "Decreases Damange Taken",
        media: "https://bafkreicoiigdpcob7h6qdyv5wa6aysowel7xurukg7ok2e2lpslzl2qmre.ipfs.nftstorage.link/",
        extra: "armour"
    }, {
        title: "Build Boost",
        description: "Reduces Build Time",
        media: "https://bafkreihsdlxom5td7e7k7xpnzxu5nqgrbld7xu4wvvtx4b65t4aamuo3hq.ipfs.nftstorage.link/",
        extra: "build"
    }];

var getRandomMetadata = () => {
    var template = { ...templates[Math.floor(Math.random() * templates.length)] };
    var boostObj = {};
    boostObj[template.extra] = 1.1 + Math.random(); //+10..110% boost
    template.extra = JSON.stringify(boostObj);

    return template;
}   