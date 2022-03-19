export class Match {

    public tier: number;
    remainingResource: number;

    players: Map<string, number> = new Map<string, number>();

    constructor(tier: number) {
        this.tier = tier;
        this.remainingResource = 1000; //depends on tier
    }

    addPlayer(id: string) {
        this.players[id] = 0;
    }

    onCollection(id: string, value: number) {
        if (this.players[id] === undefined) {
            console.error('unregistered player');
            return false;
        }

        var collected = Math.min(this.remainingResource, value);
        console.log("collected", collected);

        this.remainingResource -= collected;
        this.players[id] += collected;

        console.log("this.remainingResource", this.remainingResource);

        if (this.remainingResource < 1) {
            console.info('match ended, no more resource on map');
            return false;
        }

        return true;
    }

    distributeRemainder() {
        let playerIds = Object.keys(this.players);
        var playerCount = playerIds.length;

        for (let id of playerIds) {
            this.players[id] += this.remainingResource / playerCount;
        }

        this.remainingResource = 0;
    }
}