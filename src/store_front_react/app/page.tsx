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

  return (
  <div className="grid grid-cols-2 sm:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-6">
    <div className="bg-white aspect-[.75] rounded-xl"></div>
    <div className="bg-white aspect-[.75] rounded-xl"></div>
    <div className="bg-white aspect-[.75] rounded-xl"></div>
    <div className="bg-white aspect-[.75] rounded-xl"></div>
    <div className="bg-white aspect-[.75] rounded-xl"></div>
  </div>
  );
};

export default Home;
