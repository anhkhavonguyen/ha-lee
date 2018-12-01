export class LocationModel {
    public id: string;
    public name: string;
    public address: string;
    public type: LocationType;

    constructor(object?: any) {
        if (object) {
            this.id = object.id;
            this.name = object.name;
            this.address = object.address;
            this.type = object.type;
        }
    }
}

export class LocationViewModel extends LocationModel {
    locationType: string;
    constructor(model: LocationModel) {
        super();
        this.id = model.id;
        this.name = model.name;
        this.address = model.address;
        this.locationType = model.type === LocationType.wareHouse ? 'Warehouse' : 'Store';
    }
}

export enum LocationType {
    wareHouse = 1,
    store = 2
}

