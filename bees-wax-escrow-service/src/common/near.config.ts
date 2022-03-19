
import { KeyPair, keyStores } from "near-api-js";
import * as os from "os";
import * as fs from "fs";

const ACCOUNT_ID = 'nickbalyanitsa.testnet';
const NETWORK_ID = "testnet";
// path to your custom keyPair location (ex. function access key for example account)
const KEY_PATH = '/.near-credentials/testnet/nickbalyanitsa.testnet.json';

const credentials = JSON.parse(fs.readFileSync(os.homedir() + KEY_PATH).toString());
const keyStore = new keyStores.InMemoryKeyStore();
keyStore.setKey(NETWORK_ID, ACCOUNT_ID, KeyPair.fromString(credentials.private_key));

const RPC_API_ENDPOINT = 'https://rpc.testnet.near.org/';
const API_KEY = '19def0ca-0a9a-4556-8ec8-04617ea19894'; //.env

export const rpcConfig = {
    networkId: 'testnet',
    keyStore,
    nodeUrl: RPC_API_ENDPOINT,
    headers: { 'x-api-key': API_KEY },
};

export const archConfig = {
    keyStore,
    networkId: "testnet",
    nodeUrl: "https://archival-rpc.testnet.near.org",
    headers: {}
};