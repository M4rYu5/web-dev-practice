"use client";

import React, { useContext } from "react";
import { metadata } from "../layout";
import { BasketContext, BasketDispatchContext } from "../BasketProvider";
import { updateBasket } from "@/data/repository";

const Checkout: React.FC = () => {
  metadata.title = "Checkout";
  const basket = useContext(BasketContext);
  const setBasket = useContext(BasketDispatchContext);
  let totalPrice: number = 0;
  basket.forEach(x => {totalPrice += x.price})

  return (
<div className="rounded min-w-[400px] py-2 lg:px-10">
  <ul className="flex flex-col divide-y divide-gray-200 bg-base-100 rounded-xl">
    {basket.map((x) => (
      <li key={x.id} className="px-4 py-2 hover:bg-gray-800">
        <div className="flex justify-between items-center hover:text-white transition-colors duration-200">
          <div className="flex flex-row justify-center">
            <div className="inline-block h-20 lg:h-32 w-32 lg:w-48 overflow-clip flex-shrink-0 mr-2">
              <img
                src={x.thumbnailUrl}
                className="self-center w-full h-full object-cover p-1"
              />
            </div>
            <span className="line-clamp-2 self-center lg:text-2xl">{x.title}</span>
          </div>
          <div className="flex items-center flex-shrink-0">
            <div className="grid columns-1 flex-shrink-0 px-2 mr-2 lg:text-2xl lg:mr-6 text-center">
              <span className="text-gray-400">x{x.count}</span>
              <span className="text-sky-300">{x.price * x.count} Lei</span>
            </div>
            <button
              className="border border-gray-600 text-gray-400 rounded-full justify-around h-6 w-6 lg:h-12 lg:w-12 flex items-center"
              onClick={(e) => {
                updateBasket(basket.filter((y) => y.id != x.id)).then(
                  (y) => setBasket && setBasket(y)
                );
              }}
            >
              <svg
                xmlns="http://www.w3.org/2000/svg"
                className="h-4 w-4"
                fill="none"
                viewBox="0 0 24 24"
                stroke="currentColor"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth="2"
                  d="M6 18L18 6M6 6l12 12"
                />
              </svg>
            </button>
          </div>
        </div>
      </li>
    ))}
  </ul>
  <div className="flex justify-between bg-base-100 mt-5 px-5 py-2 rounded-xl text-2xl xl:py-4 xl:px-12 xl:text-3xl">
    <span className="self-start">Total:</span>
    <span className="">{totalPrice} Lei</span>
  </div>
</div>

  );
};

export default Checkout;
