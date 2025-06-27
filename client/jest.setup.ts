const dotenv = require("dotenv");

dotenv.config({ path: ".env.test" });

process.env.VITE_API_URL = "http://localhost:5145";
