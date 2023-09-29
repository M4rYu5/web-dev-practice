/**
 * it will format the integer part using the separator, and separate the fractional part
 * 
 * @param price the number wanted to be formated
 * @param separator the character used to separate the integer part in groups of 3
 */
export function formatPrice(price: number, separator: string): {integerPart: string, fractionalPart: string}{
    let priceInteger: number | string = Math.trunc(price); // extract integer part
    let priceFractional: number | string = Math.round((price % 1) * 100); // fractional part of the price
  
    // group with dots
    {
      let priceChars = priceInteger.toString().split("");
      for (let i = priceChars.length - 3; i > 0; i -= 3) {
        priceChars.splice(i, 0, separator);
      }
      priceInteger = priceChars.join("");
    }
  
    // hide fractional when 0
    if (priceFractional === 0) {
      priceFractional = "";
    }
    // show 0 before when fractional < 10; ex: 01 in 9.01
    else {
      priceFractional = priceFractional.toString().padStart(2, "0");
    }

    return {integerPart: priceInteger, fractionalPart: priceFractional}
}