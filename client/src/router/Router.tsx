import { createBrowserRouter } from "react-router";
import App from "../layout/App";
import HomePage from "../features/home/HomePage";
import CreateNoteForm from "../features/notes/CreateNoteForm";
import EditNoteForm from "../features/notes/EditNoteForm";
import { ProtectedRoute } from "../features/auth/ProtectedRoute";

export const router = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    children: [
      {
        index: true,
        element: (
          <ProtectedRoute>
            <HomePage />
          </ProtectedRoute>
        )
      },
      {
        path: "create",
        element: (
          <ProtectedRoute>
            <CreateNoteForm />
          </ProtectedRoute>
        )
      },
      {
        path: ":id/edit",
        element: (
          <ProtectedRoute>
            <EditNoteForm />
          </ProtectedRoute>
        )
      }
    ]
  }
]);
