import { FunctionComponent } from "react";

const ImageCard: FunctionComponent<{
  imgUrl: string;
  name: string;
  price: number;
}> = ({ imgUrl, name, price }) => {

  price = Math.ceil(price * 100) / 100; // bump up the price 9.991 to 10 and 9.881 to 9.89
  let priceInteger: number | string = Math.trunc(price); // extract integer part
  let dot: string = ","; // decimal separator
  let separator: string = ".";
  let priceFractional: number | string = Math.round((price % 1) * 100); // fractional part of the price

  // group with dots
  {
    let priceChars = priceInteger.toString().split('');
    for (let i = priceChars.length - 3; i > 0; i -= 3){
      priceChars.splice(i, 0, separator);
    }
    priceInteger = priceChars.join('');
  }

  // hide fractional when 0
  if (priceFractional === 0) {
    priceFractional = "";
    dot = "";
  }
  // show 0 before when fractional < 10; ex: 01 in 9.01
  else {
    priceFractional = priceFractional.toString().padStart(2, "0");
  }


  return (
    <div className="bg-white rounded-xl p-1 pb-4 space-y-2 flex flex-col justify-between">
      <div className="flex rounded-t-xl h-[150px] m-0 overflow-clip justify-center">
        <img
          className="rounded-t-xl self-center w-full h-full object-cover"
          src={imgUrl}
        ></img>
      </div>
      <span title={name} className="px-2 text-black font-bold min-h-[40px] line-clamp-3">{name}</span>

      <div className="flex pl-4 pr-2">
        <span className="w-4/5 text-rose-600 font-bold self-center text-lg">
          {priceInteger}
          {dot}
          <span className="align-super text-[12px]">{priceFractional}</span> Lei
        </span>
        <span className="w-1/5 cursor-pointer text-white p-1 font-bold bg-blue-500 inline-block text-center rounded-xl">
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
        </span>
      </div>
    </div>
  );
};

export default ImageCard;
