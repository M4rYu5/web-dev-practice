"use client";

import { useContext } from "react";
import { BasketContext, BasketDispatchContext } from "./BasketProvider";
import * as Repository from "@/app/_data/repository";
import { formatPrice } from "../_util/priceFormatter";

const ShopCartPreview: React.FC<React.HTMLAttributes<HTMLDivElement>> = (
  attr
) => {
  const basket = useContext(BasketContext);
  const setBasket = useContext(BasketDispatchContext);
  const pathname =
    typeof window !== "undefined" ? window.location.pathname : "/";
  let isCheckout = pathname == "/checkout";

  let totalPrice: number = 0;
  basket.forEach((x) => {
    totalPrice += x.price * x.count;
  });
  const [totalPriceInteger, totalPriceFractional] = formatPrice(
    totalPrice,
    "."
  );
  let dot = totalPriceFractional != "" ? "," : "";


  return (
    basket.length > 0 &&
    !isCheckout && (
      <div
        {...attr}
        className={attr.className + " rounded min-w-[400px] bg-base-100 py-2"}
      >
        <ul className="flex flex-col divide-y divide-neutral-content">
          {basket.map((x) => {
            let price = Math.ceil(x.price * x.count * 100) / 100; // bump up the price 9.991 to 10 and 9.881 to 9.89
            let dot: string = ","; // decimal separator
            let separator: string = ".";
            let [integerPart, fractionalPart] = formatPrice(price, separator);

            return (
              <li key={x.id} className="px-4 py-2 hover:bg-base-200">
                <div className="flex justify-between items-center transition-colors duration-200">
                  <div className="flex flex-row justify-center">
                    <div className="h-12 overflow-clip flex-shrink-0">
                      <img
                        src={x.thumbnailUrl}
                        className="self-center w-20 h-full object-cover p-1 mr-1"
                      />
                    </div>
                    <span className="line-clamp-2 self-center">{x.title}</span>
                  </div>
                  <div className="flex items-center flex-shrink-0">
                    <div className="grid columns-1 flex-shrink-0 px-2 mr-2 text-center">
                      <span className="text-gray-400 text-sm">
                        <span className="text-xs">x</span>
                        {x.count}
                      </span>
                      <span className="text-accent">
                        <span className=" font-semibold self-center text-md">
                          {integerPart}
                          {fractionalPart != "" && dot}
                          <span className="align-super text-[12px]">
                            {fractionalPart}
                          </span>{" "}
                          Lei
                        </span>
                      </span>
                    </div>
                    <button
                      className="border  border-error/30 text-error rounded-full justify-around h-6 w-6 flex items-center"
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
        <div className="flex flex-row justify-around text-accent-content w-full border-t border-neutral-content">
        <span className="self-center">Total:</span>
        <span className="self-center">
          {totalPriceInteger}
          {dot}
          <span className="text-lg self-center align-super mr-3">
            {totalPriceFractional}
          </span>
          Lei
        </span>
      </div>
      </div>
    )
  );
};

export default ShopCartPreview;
