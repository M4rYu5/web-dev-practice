"use client";

import ProductPreview from "./ProductPreview";



/**
 * Needed informations for basket (shopping cart)
 * 
 * @param id product's id
 * @param title product's name
 * @param price product's price
 * @param thumbnailUrl product's URL of display image
 * @param count the number of same products in the basket/cart
 */
export default class BasketProduct extends ProductPreview {
  /** number of units in the basket */
  count = 1;

  constructor(id: number, title: string, price: number, thumbnailUrl: string = "", count: number = 1) {
    super(id, title, price, thumbnailUrl);
    this.count = count;
  }

}
