/**
 * Used to filter the products results
 * 
 * @param skip skip first n results (useful for pagination)
 * @param take return only a specified amount of products (useful for pagiantion)
 */
export class ProductFilter {
    skip: number;
    take: number;

    constructor(skip: number = 0, take: number = 10) {
        this.skip = skip;
        this.take = take;
    }
}
