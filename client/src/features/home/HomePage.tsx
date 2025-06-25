import { CircularProgress } from "@mui/material";
import { useNotes } from "../../lib/hooks/notes/useNotes";
import NotesList from "../notes/NotesList";
import { useState } from "react";
import type { Note } from "../../lib/types";

function HomePage() {
  const { data: notes, isLoading, error } = useNotes();

  const [selectedNote, setSelectedNote] = useState<Note | null>(null);

  return (
    <div className="bg-gradient-to-r from-green-700 via-emerald-600 to-lime-500 min-h-screen flex items-center justify-center">
      <div className="bg-white bg-opacity-90 p-6 rounded-lg shadow-2xl max-w-xl w-full text-center">
        <h1 className="text-3xl font-bold mb-6 text-orange-600">My Notes</h1>
        {isLoading ? (
          <CircularProgress />
        ) : error ? (
          <p className="text-orange-600">Failed to load notes.</p>
        ) : (
          <NotesList
            notes={notes}
            selectedNote={selectedNote}
            setSelectedNote={setSelectedNote}
          />
        )}
      </div>
    </div>
  );
}

export default HomePage;
