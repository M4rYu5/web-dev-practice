export class ProductFilter {
    skip: number;
    take: number;

    constructor(skip: number = 0, take: number = 10) {
        this.skip = skip;
        this.take = take;
    }
}
