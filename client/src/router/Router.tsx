import { createBrowserRouter } from "react-router";
import App from "../layout/App";
import HomePage from "../features/home/HomePage";
import CreateNoteForm from "../features/notes/CreateNoteForm";
import EditNoteForm from "../features/notes/EditNoteForm";

export const router = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    children: [
      { index: true, element: <HomePage /> },
      { path: "create", element: <CreateNoteForm /> },
      { path: ":id/edit", element: <EditNoteForm /> }
    ]
  }
]);
