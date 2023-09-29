import { HtmlProps } from "next/dist/shared/lib/html-context";
import { Attributes, useContext, useEffect } from "react";
import { BasketContext, BasketDispatchContext } from "./BasketProvider";
import { updateBasket } from "@/data/repository";

const ShopCartPreview: React.FC<React.HTMLAttributes<HTMLDivElement>> = (
  attr
) => {
  const basket = useContext(BasketContext);
  const setBasket = useContext(BasketDispatchContext);
  let isCheckout = window.location.pathname == "/checkout";

  return (
    basket.length > 0 && !isCheckout && (
      <div
        {...attr}
        className={attr.className + " rounded min-w-[400px] bg-base-100 py-2"}
      >
        <ul className="flex flex-col divide-y divide-gray-200">
          {basket.map((x) => (
            <li key={x.id} className="px-4 py-2 hover:bg-gray-800">
              <div className="flex justify-between items-center  hover:text-white transition-colors duration-200">
                <div className="flex flex-row justify-center">
                  <div className="h-12 overflow-clip">
                    <img
                      src={x.thumbnailUrl}
                      className="self-center w-20 h-full object-cover p-1 mr-1"
                    />
                  </div>
                  <span className="line-clamp-2 self-center">{x.title}</span>
                </div>
                <div className="flex items-center">
                  <span className="text-gray-400 px-2 mr-2">{x.count}</span>
                  <button
                    className="border border-gray-600 text-gray-400 rounded-full justify-around h-6 w-6 flex items-center"
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
      </div>
    )
  );
};

export default ShopCartPreview;
