"use client";

import {} from "react";

const navigation = [
  { name: "Dashboard", href: "#", current: true },
  { name: "Team", href: "#", current: false },
  { name: "Projects", href: "#", current: false },
  { name: "Calendar", href: "#", current: false },
];

function classNames(...classes: any[]) {
  return classes.filter(Boolean).join(" ");
}

const hoverMenuClasses =
  " border-transparent cursor-pointer border-b-2 hover:shadow hover:border-white hover:border-solid hover:border-b-2 hover:text-red-400 ";

export default function SiteMenu() {
  return (
    <div className="shadow-orange-300 shadow-sm mb-2">
      <nav className="flex justify-between container mx-auto sm:px-8 px-1">
        <div className="flex">
          <a href="/" className={"pt-4 pl-4 pr-4 pb-2 mb-2" + hoverMenuClasses}>
            Home
          </a>
        </div>
        <div className="flex my-2">
          <a
            href="/checkout"
            className={
              "flex w-16 h-full content-center px-4" + hoverMenuClasses
            }
          >
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              stroke-width="1.5"
              stroke="currentColor"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                d="M2.25 3h1.386c.51 0 .955.343 1.087.835l.383 1.437M7.5 14.25a3 3 0 00-3 3h15.75m-12.75-3h11.218c1.121-2.3 2.1-4.684 2.924-7.138a60.114 60.114 0 00-16.536-1.84M7.5 14.25L5.106 5.272M6 20.25a.75.75 0 11-1.5 0 .75.75 0 011.5 0zm12.75 0a.75.75 0 11-1.5 0 .75.75 0 011.5 0z"
              />
            </svg>

            <span className="absolute text-xs px-1 self-center translate-x-4 translate-y-3 rounded-md border-solid shadow shadow-red-900 bg-gradient-to-b from-orange-300 to-red-600 text-white">
              22
            </span>
          </a>
        </div>
      </nav>
    </div>
  );
}
