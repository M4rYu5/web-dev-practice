export function getTheme(): string{
    if (typeof window === 'undefined')
      return "light";
    let pref = localStorage.getItem("site_theme_name");
      return pref ?? systemTheme();
  }
  
  function systemTheme(): string{
    return window.matchMedia("(prefers-color-scheme: dark)").matches
      ? "dark"
      : "light";
  }

  function setTheme(theme: "system" | "light" | "dark"){
    if (typeof window === "undefined") return;
    if (theme == "system") {
      localStorage.removeItem("site_theme_name");
    }
    localStorage.setItem("site_theme_name", theme);
  }