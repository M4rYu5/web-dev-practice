import { ProductFilter } from "./ProductFilter";
import { ProductPreview } from "./ProductPreview";

export function getProducts(filter: ProductFilter): Promise<ProductPreview[]> {
  return fetch("./products/products.json").then((x) => {
    return x.json();
  }).then(x =>{
    return x.slice(filter.skip, filter.skip + filter.take).map((y: any) =>{
        return new ProductPreview(y.id, y.title, y.price, y.thumbnail);
    })
  });
}

export function getProductsById(ids: number[]): Promise<ProductPreview[]>{
  return fetch("./products/products.json").then((x) => {
    return x.json();
  }).then(x =>{
    return x.filter((p: any) => ids.some(id => id == p.id)).map((y: any) =>{
        return new ProductPreview(y.id, y.title, y.price, y.thumbnail);
    })
  });
}




export function getBasket(): Promise<ProductPreview[]>{
  let jsonIDs = localStorage.getItem("user_basket");
  if (jsonIDs == null){
    return Promise.resolve([]);
  }
  let ids = JSON.parse(jsonIDs) as number[];
  return getProductsById(ids);
}

export function updateBasket(ids: number[]): Promise<ProductPreview[]>{
  localStorage.setItem("user_basket", JSON.stringify(ids));
  return getBasket();
}