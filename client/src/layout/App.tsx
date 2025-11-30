import { Outlet } from "react-router-dom";
import { AuthTokenProvider } from "../features/auth/AuthTokenProvider";

function App() {
  return (
    <AuthTokenProvider>
      <Outlet />
    </AuthTokenProvider>
  );
}

export default App;
