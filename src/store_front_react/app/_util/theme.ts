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
