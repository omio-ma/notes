import NoteItem from "./NoteItem";
import { Fab } from "@mui/material";
import AddIcon from "@mui/icons-material/Add";
import type { Note } from "../../lib/types";
import { useNavigate } from "react-router-dom";

type Props = {
  notes: Note[] | undefined;
};

function NotesList({ notes }: Props) {
  const navigate = useNavigate();

  return (
    <>
      <div className="flex justify-center mb-4">
        <Fab
          color="primary"
          aria-label="add"
          onClick={() => navigate("/create")}
        >
          <AddIcon />
        </Fab>
      </div>
      <ul className="space-y-4 transition-all duration-300">
        {notes
          ?.slice()
          .sort(
            (a, b) =>
              new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime()
          )
          .map((note) => (
            <NoteItem
              key={note.id}
              note={note}
              onClick={() => navigate(`/${note.id}/edit`)}
            />
          ))}
      </ul>
    </>
  );
}

export default NotesList;
