/** @type {import('next').NextConfig} */
const nextConfig = {
    output: 'export',
    assetPrefix: process.env.NODE_ENV === 'production' ? '/web-dev-practice/store_front_react/' : '',
}

module.exports = nextConfig
