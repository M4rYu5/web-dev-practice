"use client";

import SiteMenu from "@/app/_components/SiteMenu";
import "./globals.css";
import { Inter } from "next/font/google";
import BasketProvider, { BasketContext } from "../_components/BasketProvider";
import { useContext, useEffect, useImperativeHandle, useState } from "react";
import { getTheme } from "@/app/_util/theme";

const inter = Inter({ subsets: ["latin"] });

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  let currentTheme: string = getTheme();
  let [theme, setTeme] = useState(currentTheme);

  useEffect(() => {
    if (typeof window === "undefined") return;
    setTeme(theme);
  }, [theme]);

  useEffect(() => {
    if (typeof window !== "undefined") {
      document.title = "Store Front in React";
    }
  }, []);

  return (
    <html
      lang="en"
      data-theme={theme}
      style={{ minHeight: "100%" }}
      className="bg-base-300 bg-gradient-to-tr from-base-300"
    >
      <body
        className={
          inter.className +
          " bg-transparent bg-gradient-to-tr from-transparent text-base-content font-semibold"
        }
        style={{ minHeight: "100%" }}
      >
        <BasketProvider>
          <SiteMenu theme={theme} setTheme={setTeme} />
          <div className="container mx-auto mt-5 sm:px-12 px-5">{children}</div>
        </BasketProvider>
      </body>
    </html>
  );
}
