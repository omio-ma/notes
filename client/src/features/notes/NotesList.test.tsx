import { fireEvent, render, screen } from "@testing-library/react";
import NotesList from "./NotesList";
import type { Note } from "../../lib/types";
import * as updateHook from "../../lib/hooks/notes/useUpdateNote";

jest.mock("../../lib/hooks/notes/useUpdateNote");
jest.mock("../../lib/agent");

const mockMutate = jest.fn();
(updateHook.useUpdateNote as jest.Mock).mockReturnValue({ mutate: mockMutate });

describe("NotesList", () => {
  const mockMutate = jest.fn();

  beforeEach(() => {
    (updateHook.useUpdateNote as jest.Mock).mockReturnValue({
      mutate: mockMutate
    });
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it("renders the correct number of NoteItem components", () => {
    const mockNotes: Note[] = [
      {
        id: 1,
        title: "Note 1",
        content: "Content 1",
        createdAt: new Date().toISOString()
      },
      {
        id: 2,
        title: "Note 2",
        content: "Content 2",
        createdAt: new Date().toISOString()
      },
      {
        id: 3,
        title: "Note 3",
        content: "Content 3",
        createdAt: new Date().toISOString()
      }
    ];

    render(
      <NotesList
        notes={mockNotes}
        selectedNote={null}
        setSelectedNote={jest.fn()}
      />
    );

    mockNotes.forEach((note) => {
      expect(screen.getByTestId(`note-item-${note.id}`)).toBeDefined();
    });
  });

  it("updates input fields when typing", () => {
    const note: Note = {
      id: 1,
      title: "Original Title",
      content: "Original Content",
      createdAt: new Date().toISOString()
    };

    render(
      <NotesList
        notes={[note]}
        selectedNote={note}
        setSelectedNote={jest.fn()}
      />
    );

    const titleInput = screen.getByPlaceholderText("Title") as HTMLInputElement;
    const contentTextarea = screen.getByPlaceholderText(
      "Content"
    ) as HTMLTextAreaElement;

    fireEvent.change(titleInput, { target: { value: "Updated Title" } });
    fireEvent.change(contentTextarea, { target: { value: "Updated Content" } });

    fireEvent.click(screen.getByTestId("note-save-button"));

    expect(mockMutate).toHaveBeenCalledWith(
      {
        id: note.id,
        data: {
          title: "Updated Title",
          content: "Updated Content"
        }
      },
      expect.any(Object)
    );
  });

  it("calls mutate and then setSelectedNote(null) on success", () => {
    const note: Note = {
      id: 1,
      title: "Original Title",
      content: "Original Content",
      createdAt: new Date().toISOString()
    };

    const mockSetSelectedNote = jest.fn();

    render(
      <NotesList
        notes={[note]}
        selectedNote={note}
        setSelectedNote={mockSetSelectedNote}
      />
    );

    fireEvent.change(screen.getByPlaceholderText("Title"), {
      target: { value: "Updated Title" }
    });

    fireEvent.change(screen.getByPlaceholderText("Content"), {
      target: { value: "Updated Content" }
    });

    fireEvent.click(screen.getByTestId("note-save-button"));

    // simulating on success callback
    const mutateCallArgs = mockMutate.mock.calls[0];
    const optionsArg = mutateCallArgs[1];
    optionsArg.onSuccess();

    expect(mockSetSelectedNote).toHaveBeenCalledWith(null);
  });
});
