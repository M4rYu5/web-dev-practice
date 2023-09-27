/**
 * Data Transfer Object for basket/cart content
 */
export class BasketDTO {
  productId = 0;
  count = 0;

  constructor(productId: number, count: number) {
    this.productId = productId;
    this.count = count;
  }
}
