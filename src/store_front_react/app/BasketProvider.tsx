"use client";

import { ProductPreview } from "@/data/ProductPreview";
import { getBasket } from "@/data/repository";
import {
  ReactNode,
  useState,
  createContext,
  Dispatch,
  SetStateAction,
  experimental_useEffectEvent,
  useEffect,
} from "react";


export const BasketContext = createContext<ProductPreview[]>([]);
export const BasketDispatchContext = createContext<Dispatch<
  SetStateAction<ProductPreview[]>
> | null>(null);

const BasketProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
  const [basket, setBasket] = useState<ProductPreview[]>([]);

  useEffect(() => {
    getBasket().then(x => setBasket(x));
  }, []);

  return (
    <BasketContext.Provider value={basket}>
      <BasketDispatchContext.Provider value={setBasket}>
        {children}
      </BasketDispatchContext.Provider>
    </BasketContext.Provider>
  );
};

export default BasketProvider;
