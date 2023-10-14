"use client";

import { BasketContext } from "@/app/_components/BasketProvider";
import ShopCartPreview from "@/app/_components/ShopCartPreview";
import { Dispatch, SetStateAction, useContext } from "react";
import * as ThemeUtil from "../_util/theme";

const hoverMenuClasses =
  " border-transparent cursor-pointer border-b-2 hover:border-accent hover:border-solid hover:border-b-2 hover:text-accent ";

const SiteMenu: React.FC<{
  theme: string;
  setTheme: Dispatch<SetStateAction<ThemeUtil.Theme>>;
}> = ({ theme, setTheme: setTheme }) => {
  let basket = useContext(BasketContext);

  return (
    <div className="bg-base-300 shadow-md shadow-base-content/10 sticky top-0 z-50">
      <nav className="flex justify-between container mx-auto sm:px-8 px-1">
        <div className="flex">
          <a
            href="/"
            className={"pt-2 px-4 pb-2 my-2 font-semibold" + hoverMenuClasses}
          >
            Home
          </a>
        </div>
        <div className="flex my-2">
          <div className=" self-center justify-center mr-4 dropdown dropdown-bottom text-base-content font-semibold">
            <span
              tabIndex={0}
              className={hoverMenuClasses + "hover:border-b-0"}
            >
              {sun(true)}
              {moon(true)}
            </span>
            <ul
              tabIndex={0}
              className="dropdown-content right-[-20px] z-[1] p-2 space-y-1 bg-base-100 shadow border border-base-300 rounded-xl"
            >
              <li>
                <button
                  className="flex flex-row py-2 px-4 btn-accent w-full rounded-lg"
                  onClick={(e) => setTheme("light")}
                >
                  {sun()}&nbsp;<span className="inline-block">Light</span>
                </button>
              </li>
              <li>
                <button
                  className="flex flex-row py-2 px-4 btn-accent w-full rounded-lg"
                  onClick={(e) => setTheme("dark")}
                >
                  {moon()}&nbsp;Dark
                </button>
              </li>
              <li>
                <button
                  className="flex flex-row py-2 px-4 btn-accent w-full rounded-lg"
                  onClick={(e) => setTheme("system")}
                >
                  {computer()}&nbsp;System
                </button>
              </li>
            </ul>
          </div>
          <div className="flex self-center dropdown dropdown-hover dropdown-end dropdown-bottom">
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
                strokeWidth="1.5"
                stroke="currentColor"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  d="M2.25 3h1.386c.51 0 .955.343 1.087.835l.383 1.437M7.5 14.25a3 3 0 00-3 3h15.75m-12.75-3h11.218c1.121-2.3 2.1-4.684 2.924-7.138a60.114 60.114 0 00-16.536-1.84M7.5 14.25L5.106 5.272M6 20.25a.75.75 0 11-1.5 0 .75.75 0 011.5 0zm12.75 0a.75.75 0 11-1.5 0 .75.75 0 011.5 0z"
                />
              </svg>
              {basket.length > 0 && (
                <span className="absolute select-none text-xs px-1 self-center translate-x-4 -translate-y-3 rounded-md border-solid shadow shadow-red-900 bg-gradient-to-b from-orange-300 to-red-600 text-white">
                  {basket.length}
                </span>
              )}
            </a>
            <ShopCartPreview className="dropdown-content z-[1]" />
          </div>
        </div>
      </nav>
    </div>
  );
};

function sun(autoHide: boolean = false) {
  return (
    <svg
      xmlns="http://www.w3.org/2000/svg"
      fill="none"
      viewBox="0 0 24 24"
      strokeWidth={1.5}
      stroke="currentColor"
      className={"inline-block w-6 h-6 " + (autoHide ? "show-in-light" : "")}
    >
      <path
        strokeLinecap="round"
        strokeLinejoin="round"
        d="M12 3v2.25m6.364.386l-1.591 1.591M21 12h-2.25m-.386 6.364l-1.591-1.591M12 18.75V21m-4.773-4.227l-1.591 1.591M5.25 12H3m4.227-4.773L5.636 5.636M15.75 12a3.75 3.75 0 11-7.5 0 3.75 3.75 0 017.5 0z"
      />
    </svg>
  );
}

function moon(autoHide: boolean = false) {
  return (
    <svg
      xmlns="http://www.w3.org/2000/svg"
      fill="none"
      viewBox="0 0 24 24"
      strokeWidth={1.5}
      stroke="currentColor"
      className={"inline-block w-6 h-6 " + (autoHide ? "show-in-dark" : "")}
    >
      <path
        strokeLinecap="round"
        strokeLinejoin="round"
        d="M21.752 15.002A9.718 9.718 0 0118 15.75c-5.385 0-9.75-4.365-9.75-9.75 0-1.33.266-2.597.748-3.752A9.753 9.753 0 003 11.25C3 16.635 7.365 21 12.75 21a9.753 9.753 0 009.002-5.998z"
      />
    </svg>
  );
}

function computer() {
  return (
    <svg
      xmlns="http://www.w3.org/2000/svg"
      fill="none"
      viewBox="0 0 24 24"
      strokeWidth={1.5}
      stroke="currentColor"
      className="inline-block w-6 h-6"
    >
      <path
        strokeLinecap="round"
        strokeLinejoin="round"
        d="M9 17.25v1.007a3 3 0 01-.879 2.122L7.5 21h9l-.621-.621A3 3 0 0115 18.257V17.25m6-12V15a2.25 2.25 0 01-2.25 2.25H5.25A2.25 2.25 0 013 15V5.25m18 0A2.25 2.25 0 0018.75 3H5.25A2.25 2.25 0 003 5.25m18 0V12a2.25 2.25 0 01-2.25 2.25H5.25A2.25 2.25 0 013 12V5.25"
      />
    </svg>
  );
}

export default SiteMenu;
