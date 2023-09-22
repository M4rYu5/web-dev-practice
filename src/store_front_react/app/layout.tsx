import SiteMenu from "@/components/SiteMenu";
import "./globals.css";
import type { Metadata } from "next";
import { Inter } from "next/font/google";

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
    <html lang="en" style={{ minHeight: "100%" }}>
      <body className={inter.className}>
        <SiteMenu />
        <div className="container mx-auto mt-5 sm:px-12 px-5">
          {children}
        </div>
      </body>
    </html>
  );
}
