const filePrefix = process.env.NODE_ENV === 'production' ? '/web-dev-practice/store_front_react' : '';


/**
 * Prefix the path with the actual location. 
 * If in production, the site will be in a file, not at root directory
 * so all links should be prefixed with site location.
 * @param path url path that needs to be transformed
 * @returns returns the actual path
 */
export function url_p(path: string): string {
    // Ensure the path starts with a "/"
    if (!path.startsWith('/')) {
        path = '/' + path;
    }

    // Concatenate the assetPrefix with the path
    return filePrefix + path;
}