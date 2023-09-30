"use client";

import React, { useContext } from "react";
import { metadata } from "../layout";
import { BasketContext, BasketDispatchContext } from "../BasketProvider";
import * as Repository from "@/data/repository";
import { formatPrice } from "../util/priceFormatter";
import BasketProduct from "@/data/BasketProduct";

function increaseCount(
  bag: BasketProduct[],
  setBag: React.Dispatch<React.SetStateAction<BasketProduct[]>> | null,
  index: number,
  amount: number
) {
  const _product = bag.filter((x) => x.id == index);
  if (_product == null) return;
  const product = _product[0];
  if (product.count + amount < 1) return;
  product.count = product.count + amount;
  let b = [...bag];
  Repository.updateBasket(b);
  setBag && setBag(b);
}

const Checkout: React.FC = () => {
  metadata.title = "Checkout";
  const basket = useContext(BasketContext);
  const setBasket = useContext(BasketDispatchContext);
  let totalPrice: number = 0;
  basket.forEach((x) => {
    totalPrice += x.price * x.count;
  });

  const [totalPriceInteger, totalPriceFractional] = formatPrice(totalPrice, ".");
  let dot = totalPriceFractional != "" ? "," : "";

  return (
    <div className="min-w-[400px] py-2 lg:px-10">
      <ul className="flex flex-col divide-y divide-neutral-content bg-base-100 rounded-xl overflow-clip shadow-md">
        {basket.map((x) => {
          let price = Math.ceil(x.price * x.count * 100) / 100; // bump up the price 9.991 to 10 and 9.881 to 9.89
          let dot: string = ","; // decimal separator
          let separator: string = ".";
          let [integerPart, fractionalPart] = formatPrice(price, separator);

          return (
            <li key={x.id} className="px-4 py-2 hover:bg-base-200">
              <div className="flex justify-between items-center text-accent-content transition-colors duration-200">
                <div className="flex flex-row justify-center">
                  <div className="inline-block h-20 lg:h-32 w-32 lg:w-48 overflow-clip flex-shrink-0 mr-2">
                    <img
                      src={x.thumbnailUrl}
                      className="self-center w-full h-full object-cover p-1"
                    />
                  </div>
                  <span className="line-clamp-2 self-center lg:text-2xl">
                    {x.title}
                  </span>
                </div>
                <div className="flex items-center flex-shrink-0">
                  <div className="grid columns-1 flex-shrink-0 px-2 mr-2 lg:text-xl lg:mr-6 text-center">
                    <div className="flex flex-row space-x-1 m-auto">
                      <button
                        className="rounded-full m-auto p-[2px] cursor-pointer hover:text-secondary"
                        onClick={(e) =>
                          increaseCount(basket, setBasket, x.id, -1)
                        }
                      >
                        <svg
                          xmlns="http://www.w3.org/2000/svg"
                          fill="none"
                          viewBox="0 0 24 24"
                          strokeWidth="1.5"
                          stroke="currentColor"
                          className="w-5 h-5"
                        >
                          <path
                            strokeLinecap="round"
                            strokeLinejoin="round"
                            d="M15.75 19.5L8.25 12l7.5-7.5"
                          />
                        </svg>
                      </button>
                      <span className="text-gray-400">x{x.count}</span>
                      <button
                        className="rounded-full m-auto p-[2px] cursor-pointer hover:text-secondary"
                        onClick={(e) =>
                          increaseCount(basket, setBasket, x.id, 1)
                        }
                      >
                        <svg
                          xmlns="http://www.w3.org/2000/svg"
                          fill="none"
                          viewBox="0 0 24 24"
                          strokeWidth="1.5"
                          stroke="currentColor"
                          className="w-5 h-5"
                        >
                          <path
                            strokeLinecap="round"
                            strokeLinejoin="round"
                            d="M8.25 4.5l7.5 7.5-7.5 7.5"
                          />
                        </svg>
                      </button>
                    </div>
                    <span className="text-accent">
                      {integerPart}
                      {fractionalPart != "" && dot}
                      <span className="align-super text-[12px]">
                        {fractionalPart}
                      </span>{" "}
                      Lei
                    </span>
                  </div>
                  <button
                    className="border border-error/30 text-error rounded-full justify-around h-6 w-6 lg:h-12 lg:w-12 flex items-center"
                    onClick={(e) => {
                      Repository.updateBasket(
                        basket.filter((y) => y.id != x.id)
                      ).then((y) => setBasket && setBasket(y));
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
          );
        })}
      </ul>
      <div className="flex justify-between bg-base-100 text-accent-content mt-5 px-5 py-2 rounded-xl text-2xl xl:py-4 xl:px-12 xl:text-3xl  shadow-md">
        <span className="self-start">Total:</span>
        <span className="">
          {totalPriceInteger}
          {dot}
          <span className="text-lg align-super mr-3">
            {totalPriceFractional}
          </span>
          Lei
        </span>
      </div>
    </div>
  );
};

export default Checkout;
