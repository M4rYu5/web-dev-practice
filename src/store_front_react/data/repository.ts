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
