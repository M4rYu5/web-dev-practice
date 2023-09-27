import { BasketProduct } from "@/data/BasketProduct";
import { ProductFilter } from "./ProductFilter";
import { ProductPreview } from "./ProductPreview";
import { BasketDTO } from "./BasketDTO";

// ---------  PRODUCTS  ------------

export function getProducts(filter: ProductFilter): Promise<ProductPreview[]> {
  return fetch("./products/products.json")
    .then((x) => {
      return x.json();
    })
    .then((x) => {
      return x.slice(filter.skip, filter.skip + filter.take).map((y: any) => {
        return new ProductPreview(y.id, y.title, y.price, y.thumbnail);
      });
    });
}

export function getProductsById(ids: number[]): Promise<ProductPreview[]> {
  return fetch("./products/products.json")
    .then((x) => {
      return x.json();
    })
    .then((x) => {
      return x
        .filter((p: any) => ids.some((id) => id == p.id))
        .map((y: any) => {
          return new ProductPreview(y.id, y.title, y.price, y.thumbnail);
        });
    });
}

// ---------  BASKET  ------------

export function getBasket(): Promise<BasketProduct[]> {
  let jsonBasket = localStorage.getItem("user_basket");
  if (jsonBasket == null) {
    return Promise.resolve([]);
  }

  // this should've been done on server, receiving the accuret products details
  let basket = JSON.parse(jsonBasket) as BasketDTO[];
  return getProductsById(basket.map((x) => x.productId)).then((products) => {
    return Promise.resolve(
      products.map((x) => {
        let bp = basket.find((y) => y.productId == x.id);
        return new BasketProduct(
          x.id,
          x.title,
          x.price,
          x.thumbnailUrl,
          bp?.count
        );
      })
    );
  });
}

export function updateBasket(
  products: BasketProduct[]
): Promise<BasketProduct[]> {
  localStorage.setItem(
    "user_basket",
    JSON.stringify(products.map((x) => new BasketDTO(x.id, x.count)))
  );
  return getBasket();
}
