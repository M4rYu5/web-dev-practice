"use client";

import { getBasket } from "@/app/_data/repository";
import {
  ReactNode,
  useState,
  createContext,
  Dispatch,
  SetStateAction,
  useEffect,
} from "react";
import BasketProduct from "../_data/types/BasketProduct";



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
