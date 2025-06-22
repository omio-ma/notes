import { CircularProgress } from "@mui/material";
import "./App.css";
import type { Note } from "./lib/types";
import { useNotes } from "./lib/hooks/notes/useNotes";

function App() {
  const { data: notes, isLoading, error } = useNotes();

  return (
    <div className="bg-gradient-to-r from-yellow-400 via-amber-300 to-orange-400 min-h-screen flex items-center justify-center">
      <div className="bg-white bg-opacity-90 p-6 rounded-lg shadow-lg max-w-xl w-full text-center">
        <h1 className="text-3xl font-bold mb-6 text-orange-700">My Notes</h1>
        {isLoading ? (
          <CircularProgress />
        ) : error ? (
          <p className="text-red-600">Failed to load notes.</p>
        ) : (
          <ul className="space-y-4">
            {notes?.map((note: Note) => (
              <li
                key={note.id}
                className="p-4 rounded-md border border-orange-200 bg-orange-50"
              >
                <p className="text-lg font-medium text-orange-800">
                  {note.title}
                </p>
              </li>
            ))}
          </ul>
        )}
      </div>
    </div>
  );
}

export default App;
