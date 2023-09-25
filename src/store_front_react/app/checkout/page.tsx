"use client"

import React from 'react';
import { metadata } from '../layout';



const Checkout: React.FC = (params) => {
  metadata.title = "Checkout";

  return (
      <h1>Checkout: {JSON.stringify(params)}</h1>
  );
};

export default Checkout;