"use client";

import SiteMenu from "@/app/_components/SiteMenu";
import "../globals.css";
import { Inter } from "next/font/google";
import BasketProvider from "../_components/BasketProvider";
import { useEffect, useState } from "react";
import * as ThemeUtil from "@/app/_util/theme";

const inter = Inter({ subsets: ["latin"] });

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  // Note: the run dev mode will render the page on server, even if the output is set to export, and so
  //   the currentTheme will be "light", even if the console.log(currentTheme) will print the actual value
  let currentTheme: ThemeUtil.Theme = ThemeUtil.getConcreteTheme();
  let [theme, setTheme] = useState<ThemeUtil.Theme>(currentTheme);

  useEffect(() => {
    ThemeUtil.saveTheme(theme);
  }, [theme]);

  useEffect(() => {
    if (typeof window !== "undefined") {
      document.title = "Store Front in React";
    }
  }, []);

  return (
    <html
      lang="en"
      data-theme={ThemeUtil.convertTheme(theme)}
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
          <SiteMenu theme={theme} setTheme={setTheme} />
          <div className="container mx-auto mt-5 sm:px-12 px-5">{children}</div>
        </BasketProvider>
      </body>
    </html>
  );
}
