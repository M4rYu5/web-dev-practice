export type ConcreteTheme = "light" | "dark";
export type Theme = "system" | ConcreteTheme;

export function convertTheme(theme: Theme): ConcreteTheme {
  switch (theme) {
    case "dark":
      return "dark";
    case "light":
      return "light";
    case "system":
      return getSystemTheme();
  }
}

export function getConcreteTheme(): ConcreteTheme {
  if (typeof window === "undefined") {
    return "light";
  }
  let pref = localStorage.getItem("site_theme_name") as ConcreteTheme | null;
  return pref ?? getSystemTheme();
}

export function getSystemTheme(): ConcreteTheme {
  return typeof window == "undefined"
    ? "light"
    : window.matchMedia("(prefers-color-scheme: dark)").matches
    ? "dark"
    : "light";
}

export function saveTheme(theme: Theme) {
  if (typeof window === "undefined") return;
  if (theme == "system") {
    localStorage.removeItem("site_theme_name");
  }
  localStorage.setItem("site_theme_name", theme);
}

/***
 * Solves the initial theme change problem, from white to black, by setting the data-theme in JavaScript.
 * this problem was caused by the server rendering first frame, without knowing the user's preference in theme.
 * the white theme is the default one when the localStorage is not accesible (like on server. When user uses
 * a dark theme the client will have to change it to dark (using useEffect) but is too late because the
 * light page already displayed.
 */
export function ThemeInitialSetScript() {
  return (
    <script
      dangerouslySetInnerHTML={{
        __html: `
          let clientPref = window.matchMedia("(prefers-color-scheme: dark)").matches ? "dark" : "light";
          let userPref; 
          try{
            userPref = localStorage.getItem("site_theme_name");
          }
          catch(e){}

          let theme = (userPref == null  || userPref == "system")  ? clientPref : userPref;
          if (theme != null){
            document.documentElement.removeAttribute("data-theme");
            document.documentElement.setAttribute("data-theme", theme);
          }
        `,
      }}
    ></script>
  );
}
