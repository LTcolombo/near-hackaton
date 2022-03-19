import { Server, Socket } from "socket.io";
import { getCollection } from "../db/deposits";
import { Match } from "./match";

export class Escrow {

    matchLookup = new Map<string, string>();
    matches = new Map<string, Match>();
    sockets = new Map<string, Socket>();

    constructor() {
        let io = new Server(8083, { cors: { origin: '*' }, allowEIO3: true });
        console.log("http server instance created..");

        io.on("connection", (socket) => {
            console.log(socket.id + " connected..");

            socket.on("start", this.onStart.bind(this, socket));
            socket.on("collection", this.onCollection.bind(this, socket));
            socket.on("disconnect", this.onDisconnect.bind(this, socket));

            this.sockets[socket.id] = socket;
        });
    }

    onStart(socket: Socket, json: any) {
        var data = JSON.parse(json);
        console.log("start", data, data.matchId);


        if (!this.matches[data.matchId])
            this.matches[data.matchId] = new Match(data.tier);

        if (this.matches[data.matchId].tier != data.tier) { //previously created match has different tier
            console.error('wrong tier! debug matchmaker');
            return;
        }

        console.log(Object.keys(this.matches));

        this.matches[data.matchId].addPlayer(socket.id);
        this.matchLookup[socket.id] = data.matchId;

        console.log("start", JSON.stringify(this.matches[this.matchLookup[socket.id]]));
        socket.emit("start", JSON.stringify(this.matches[data.matchId].remainingResource));
    }

    onCollection(socket: Socket, value: number) {
        console.log("collection", value);

        console.log(Object.keys(this.matches));
        console.log(this.matchLookup[socket.id]);

        if (!this.matchLookup[socket.id] || !this.matches[this.matchLookup[socket.id]]) {
            console.error('match not created for player');
            return;
        }

        if (!this.matches[this.matchLookup[socket.id]].onCollection(socket.id, value)) {
            this.endMatch(this.matchLookup[socket.id]);
        }

        console.log("res", JSON.stringify(this.matches[this.matchLookup[socket.id]]));
        socket.emit("res", JSON.stringify(this.matches[this.matchLookup[socket.id]]));
    }

    async endMatch(id: string) {
        var value: Match = this.matches[id];
        if (!value) {
            console.info('match aready ended');
            return;
        }

        value.distributeRemainder();

        var playerIds = Object.keys(value.players);

        let initialStake = 1000 / playerIds.length; //1000 should depend on tier
        for (let playerId of playerIds) {

            const deposit = (await (await getCollection()).findOne({ wallet: playerId }));
            if (deposit)
                console.log(await (await getCollection()).updateOne({ wallet: playerId }, { $inc: { value: Math.round(value.players[playerId] - initialStake) } }, true));
            else
                console.error("add a deposit check on server side");

            this.sockets[playerId].emit("end", value.players[playerId]);
        }

        delete this.matches[id];
    }

    onDisconnect(socket: Socket) {
        this.endMatch(this.matches[this.matchLookup[socket.id]]);
    }

}
