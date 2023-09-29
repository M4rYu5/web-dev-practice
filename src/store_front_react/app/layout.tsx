import SiteMenu from "@/components/SiteMenu";
import "./globals.css";
import type { Metadata } from "next";
import { Inter } from "next/font/google";
import BasketProvider, { BasketContext } from "./BasketProvider";

const inter = Inter({ subsets: ["latin"] });

export const metadata: Metadata = {
  title: "Store Front in React",
  description: "E-Commerce front-end written in React",
};

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="en" style={{ minHeight: "100%"}} className="bg-black bg-gradient-to-tr from-black">
      <body className={inter.className + " bg-transparent bg-gradient-to-tr from-transparent"} style={{ minHeight: "100%"}}>
        <BasketProvider>
          <SiteMenu />
          <div className="container mx-auto mt-5 sm:px-12 px-5">{children}</div>
        </BasketProvider>
      </body>
    </html>
  );
}
