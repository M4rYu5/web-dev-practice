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
    <div className="bg-white rounded-xl p-1 pb-4 space-y-2">
      <img className="rounded-t-xl" src="https://i.dummyjson.com/data/products/1/thumbnail.jpg"></img>
      <span className="block px-2 text-black font-bold">iPhone 9</span>
      <div className="flex pl-4 pr-2">
        <span className="w-4/5 text-rose-600 font-bold self-center">99.<span className="align-top text-[12px]">99</span> Lei</span>
        <span className="w-1/5 cursor-pointer text-white p-1 font-bold bg-blue-500 inline-block text-center rounded-xl">
          <svg
                xmlns="http://www.w3.org/2000/svg"
                fill="none"
                viewBox="0 0 24 24"
                strokeWidth="1.5"
                stroke="currentColor"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  d="M2.25 3h1.386c.51 0 .955.343 1.087.835l.383 1.437M7.5 14.25a3 3 0 00-3 3h15.75m-12.75-3h11.218c1.121-2.3 2.1-4.684 2.924-7.138a60.114 60.114 0 00-16.536-1.84M7.5 14.25L5.106 5.272M6 20.25a.75.75 0 11-1.5 0 .75.75 0 011.5 0zm12.75 0a.75.75 0 11-1.5 0 .75.75 0 011.5 0z"
                />
              </svg>
        </span>
      </div>
    </div>

  </div>
  );
};

export default Home;
