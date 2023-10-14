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
  // to remove the initial theme change (because the server will compile first frame)
  // initial theme is also set by <ThemeUtil.ThemeInitialSetScript />
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
      className="bg-base-300"
    >
      <body
        className={
          inter.className +
          "text-base-content font-semibold"
        }
        style={{ minHeight: "100%" }}
      >
        {/* solves the initial theme change by applying it in JavaScript*/}
        <ThemeUtil.ThemeInitialSetScript />
        <BasketProvider>
          <SiteMenu theme={theme} setTheme={setTheme} />
          <div className="container mx-auto mt-5 sm:px-12 px-5">{children}</div>
        </BasketProvider>
      </body>
    </html>
  );
}
