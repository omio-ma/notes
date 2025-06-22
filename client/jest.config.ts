import type { Config } from "jest";

const config: Config = {
  preset: "ts-jest",
  testEnvironment: "jsdom",
  setupFiles: ["<rootDir>/jest.setup.ts"],
  moduleNameMapper: {
    "^src/(.*)$": "<rootDir>/src/$1"
  },
  globals: {
    "ts-jest": {
      useESM: false,
      tsconfig: "<rootDir>/tsconfig.node.json"
    }
  }
};

export default config;
