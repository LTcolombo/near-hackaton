import { Db, MongoClient } from "mongodb";
import * as dotenv from "dotenv";

export const getCollection = async () => {
    if (!collection)
        await connectToDatabase();
    return collection;

}

var collection;
export async function connectToDatabase() {

    dotenv.config();

    const client = new MongoClient(process.env.DB_CONN_STRING);

    await client.connect();
    const db: Db = client.db(process.env.DB_NAME);
    collection = db.collection("deposits");
    console.log(`Successfully connected to database: ${db.databaseName} and collection: ${collection.collectionName}`);

    return true;
}