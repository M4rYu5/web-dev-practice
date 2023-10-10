"use client";

import { Dispatch, SetStateAction, useEffect, useMemo, useState } from "react";
import { getProducts } from "../_data/repository";
import { ProductFilter } from "@/app/_data/types/ProductFilter";
import ProductPreview from "@/app/_data/types/ProductPreview";
import ProductCard from "../_components/ProductCard";

const Home = () => {
  let [filter, setFilter] = useState(new ProductFilter(0, 20));
  let [products, setProducts]: [ProductPreview[] | null, any] = useState<
    ProductPreview[] | null
  >(null);

  useEffect(() => {
    getProducts(filter).then((x) => setProducts(x));
  }, [filter]);

  return (
    <div className="grid grid-cols-2 sm:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-6">
      {products?.map((x, i) => {
        return (
          <ProductCard
            key={x.id}
            id={x.id}
            imgUrl={x.thumbnailUrl}
            price={x.price}
            name={x.title}
          />
        );
      })}
    </div>
  );
};

export default Home;
