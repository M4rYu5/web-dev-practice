"use client";

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
import { BasketProduct } from "../data/BasketProduct";



export const BasketContext = createContext<BasketProduct[]>([]);
export const BasketDispatchContext = createContext<Dispatch<
  SetStateAction<BasketProduct[]>
> | null>(null);

const BasketProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
  const [basket, setBasket] = useState<BasketProduct[]>([]);

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
