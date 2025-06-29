import type { ReactNode } from "react";

type Props = {
  children: ReactNode;
};

function Layout({ children }: Props) {
  return (
    <div className="bg-gradient-to-r from-green-700 via-emerald-600 to-lime-500 min-h-screen flex items-center justify-center">
      <div className="bg-white bg-opacity-90 p-6 rounded-lg shadow-2xl max-w-xl w-full text-center">
        {children}
      </div>
    </div>
  );
}

export default Layout;
