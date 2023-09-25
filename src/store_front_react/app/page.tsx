"use client";

import { Dispatch, SetStateAction, useEffect, useMemo, useState } from "react";
import { getProducts } from "./../data/repository";
import { ProductFilter } from "@/data/ProductFilter";
import { ProductPreview } from "@/data/ProductPreview";

const Home = () => {
  let [filter, setFilter] = useState(new ProductFilter(0, 10));
  let [products, setProducts]: [ProductPreview[] | null, any] = useState(null);

  useEffect(() => {
    getProducts(filter).then((x) => setProducts(x));
  }, [filter]);

  return <div>{JSON.stringify(products)}</div>;
};

export default Home;
