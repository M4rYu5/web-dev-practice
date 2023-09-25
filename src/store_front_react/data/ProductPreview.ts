
export class ProductPreview {
    id: number = -1;
    title: string = "";
    price: number = Number.MAX_SAFE_INTEGER;
    thumbnailUrl: string = "";

    constructor(id: number, title: string, price: number, thumbnailUrl: string = "") {
        this.id = id;
        this.title = title;
        this.price = price;
        this.thumbnailUrl = thumbnailUrl;
    }
}
