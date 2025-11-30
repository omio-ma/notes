import type { ReactNode } from "react";
import { Box, Paper, Stack } from "@mui/material";
import { UserProfile } from "../features/auth/UserProfile";
import { LogoutButton } from "../features/auth/LogoutButton";

type Props = {
  children: ReactNode;
};

function Layout({ children }: Props) {
  return (
    <div className="bg-gradient-to-r from-green-700 via-emerald-600 to-lime-500 min-h-screen flex items-center justify-center">
      <div className="bg-white bg-opacity-90 p-6 rounded-lg shadow-2xl max-w-xl w-full text-center">
        <Paper elevation={0} sx={{ mb: 3, p: 2, backgroundColor: 'transparent' }}>
          <Stack direction="row" justifyContent="space-between" alignItems="center">
            <UserProfile />
            <LogoutButton />
          </Stack>
        </Paper>
        {children}
      </div>
    </div>
  );
}

export default Layout;
