
/**
 * Needed informations for prodcut preview
 * 
 * @param id product's id
 * @param title product's name
 * @param price product's price
 * @param thumbnailUrl product's URL of display image
 */
export default class ProductPreview {
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
